import { Button, Modal, FloatingLabel, Form } from 'react-bootstrap';
import { useState } from 'react';
import { MatchDetailProps } from '../slices/matchDetailSlice';
import { MemberProps } from '../slices/memberSlice';

export default function AddMatchDetail(props: { show: boolean, members: MemberProps[], matchId: number, onSubmit: (model: MatchDetailProps) => void, onClose: () => void }) {
    const { matchId, members, show, onSubmit, onClose } = props
    const [detail, setDetail] = useState<MatchDetailProps>({ id: 0, matchId: matchId, memberId: 0, isPaid: false, isSkip: false })
    const onValid = () => {
        onSubmit(detail)
    }

    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Add a match detail</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="addMatch.Member">
                        <FloatingLabel controlId="floatingMember" label="Member">
                            <Form.Select value={detail.memberId} onChange={(e) => setDetail({ ...detail, memberId: parseInt(e.target.value) })}>
                                <option hidden value={0}>Select a member</option>
                                {
                                    members.map((value, index) =>
                                        <option key={`teamcount.id-${index}`} value={value.id}>{value.name}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                        <Form.Check className='my-2' label="Is Paid" type='switch' value={detail.isPaid ? 1 : 0}
                            onChange={(e) => setDetail({ ...detail, isPaid: e.target.checked })} />
                        <Form.Check label="Is Skip" type='switch' value={detail.isSkip ? 1 : 0}
                            onChange={(e) => setDetail({ ...detail, isSkip: e.target.checked })} />
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={onValid}>
                    Add
                </Button>
            </Modal.Footer>
        </Modal >
    </>)
}