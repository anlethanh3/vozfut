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
                <Modal.Title>Exchange players</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="addMatch.PlayerIn">
                        <FloatingLabel controlId="floatingPlayerIn" label="PlayerIn">
                            <Form.Select value={exchange.memberInId} onChange={(e) => setMatch({ ...exchange, memberInId: parseInt(e.target.value) })}>
                                <option hidden value={0}>Select a Player In</option>
                                {
                                    members.map(value =>
                                        <option key={`playerIn.id-${value.id}`} value={value.id}>{value.name}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="addMatch.playerOut">
                        <FloatingLabel controlId="floatingplayerOut" label="playerOut">
                            <Form.Select value={exchange.memberOutId} onChange={(e) => setMatch({ ...exchange, memberOutId: parseInt(e.target.value) })}>
                                <option hidden value={0}>Select a Player Out</option>
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
                    Exchange
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}