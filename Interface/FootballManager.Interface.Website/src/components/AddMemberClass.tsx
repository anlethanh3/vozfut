import { Button, Modal, FloatingLabel, Form } from 'react-bootstrap';
import { useState } from 'react';
import { MemberProps } from '../slices/memberSlice';

export interface AddClassMemberProps {
    memberId: number,
    classId: number,
}

export default function AddMemberClass(props: { show: boolean, members: MemberProps[], onSubmit: (data: AddClassMemberProps) => void, onClose: () => void }) {
    const { show, onSubmit, onClose, members } = props
    const [member, setMember] = useState<AddClassMemberProps>({ classId: 2, memberId: -1 })
    const onValid = () => {
        if (member.memberId === -1 || member.classId === -1) {
            return
        }
        onSubmit(member)
    }

    return (<>
        <Modal show={show} onHide={onClose}>
            <Modal.Header closeButton>
                <Modal.Title>Add Member Class</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="updateRivalMember.ClassId">
                        <FloatingLabel controlId="floatingClassId" label="Team">
                            <Form.Select value={member.classId} onChange={(e) => setMember({ ...member, classId: parseInt(e.target.value) })}>
                                <option hidden value={-1}>Select Class</option>
                                {
                                    ["Class SSS", "Class S", "Class A"].map((value, index) =>
                                        <option key={`ClassId.id-${index}`} value={index}>{value}</option>
                                    )
                                }
                            </Form.Select>
                        </FloatingLabel>
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="updateRivalMember.MemberId">
                        <FloatingLabel controlId="floatingMemberId" label="Member In">
                            <Form.Select value={member.memberId} onChange={(e) => setMember({ ...member, memberId: parseInt(e.target.value) })}>
                                <option hidden value={0}>Select Member</option>
                                {
                                    members.map(value =>
                                        <option key={`MemberId.id-${value.id}`} value={value.id}>{value.name}</option>
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
                    Confirm
                </Button>
            </Modal.Footer>
        </Modal>
    </>)
}