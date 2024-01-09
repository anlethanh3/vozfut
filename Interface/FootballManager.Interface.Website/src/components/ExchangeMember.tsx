import { Button, Modal, FloatingLabel, Form } from 'react-bootstrap';
import { useEffect, useState } from 'react';
import { ExchangeMemberProps } from '../slices/matchDetailSlice';
import { MemberProps } from '../slices/memberSlice';

export default function ExchangeMember(props: { show: boolean, members: MemberProps[], onSubmit: (data: ExchangeMemberProps) => void, onClose: () => void }) {
    const { show, onSubmit, onClose, members, } = props
    const [exchange, setMatch] = useState<ExchangeMemberProps>({ matchId: 0, memberInId: 0, memberOutId: 0 })
    const onValid = () => {
        if (exchange.memberInId === 0 && exchange.memberOutId === 0) {
            return
        }
        onSubmit(exchange)
    }

    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Swap Member</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="addMatch.MemberIn">
                        <FloatingLabel controlId="floatingMemberIn" label="Member 1">
                            <Form.Select value={exchange.memberInId} onChange={(e) => setMatch({ ...exchange, memberInId: parseInt(e.target.value) })}>
                                <option hidden value={0}>Select a Member 1</option>
                                {
                                    members.map(value =>
                                        <option key={`MemberIn.id-${value.id}`} value={value.id}>{value.name}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMatch.MemberOut">
                        <FloatingLabel controlId="floatingMemberOut" label="Member 2">
                            <Form.Select value={exchange.memberOutId} onChange={(e) => setMatch({ ...exchange, memberOutId: parseInt(e.target.value) })}>
                                <option hidden value={0}>Select a Member 2</option>
                                {
                                    members.map(value =>
                                        <option key={`playerout.id-${value.id}`} value={value.id}>{value.name}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={onValid}>
                    Swap
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}